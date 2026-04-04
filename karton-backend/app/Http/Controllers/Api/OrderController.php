<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Models\Order;
use App\Models\OrderItem;
use App\Models\Product;
use App\Models\StockMovement;
use Illuminate\Support\Facades\DB;

class OrderController extends Controller
{
    public function index()
    {
        return Order::with('items.product')->get();
    }

    public function store(Request $request)
    {
        $request->validate([
            'user_id'=>'required|exists:users,id',
            'items'=>'required|array'
        ]);

        DB::transaction(function() use ($request, &$order){
            $order = Order::create([
                'user_id'=>$request->user_id,
                'total_price'=>0
            ]);

            $total = 0;

            foreach($request->items as $item){
                $product = Product::with('ingredients')->findOrFail($item['product_id']);
                $quantity = $item['quantity'];

                OrderItem::create([
                    'order_id'=>$order->id,
                    'product_id'=>$product->id,
                    'quantity'=>$quantity
                ]);

                $total += $product->price * $quantity;

                foreach($product->ingredients as $ingredient){
                    $used = $ingredient->pivot->quantity * $quantity;
                    $inventory = $ingredient->inventory;

                    if(!$inventory || $inventory->quantity < $used){
                        throw new \Exception("Not enough {$ingredient->name} in stock");
                    }

                    $inventory->decrement('quantity', $used);

                    StockMovement::create([
                        'ingredient_id'=>$ingredient->id,
                        'change_amount'=>-$used,
                        'reason'=>'order'
                    ]);
                }
            }

            $order->total_price = $total;
            $order->save();
        });

        return response()->json($order->load('items.product'),201);
    }

    public function show($id)
    {
        return Order::with('items.product')->findOrFail($id);
    }

    public function getByDate($date)
    {
        return Order::with('items.product')
            ->whereDate('created_at', $date)
            ->get();
    }
}
