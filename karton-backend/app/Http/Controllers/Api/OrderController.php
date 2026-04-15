<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Models\Order;
use App\Models\OrderItem;
use App\Models\Product;
use App\Models\Inventory;
use App\Models\StockMovement;
use Illuminate\Support\Facades\DB;

class OrderController extends Controller
{
    public function index()
    {
        $orders = Order::with('items.product')->latest()->get();

        return response()->json([
            'success' => true,
            'data' => $orders
        ]);
    }

    public function show($id)
    {
        $order = Order::with('items.product')->findOrFail($id);

        return response()->json([
            'success' => true,
            'data' => $order
        ]);
    }

    public function store(Request $request)
    {
        $request->validate([
            'items' => 'required|array|min:1',
            'items.*.product_id' => 'required|exists:products,id',
            'items.*.quantity' => 'required|numeric|min:1'
        ]);

        DB::beginTransaction();

        try {
            $order = Order::create([
                'user_id' => $request->user()->id,
                'status' => 'completed'
            ]);

            foreach ($request->items as $item) {

                $product = Product::with('ingredients')->findOrFail($item['product_id']);

                OrderItem::create([
                    'order_id' => $order->id,
                    'product_id' => $product->id,
                    'quantity' => $item['quantity'],
                    'price' => $product->price
                ]);

                foreach ($product->ingredients as $ingredient) {

                    $neededAmount = $ingredient->pivot->quantity * $item['quantity'];

                    $inventory = Inventory::where('ingredient_id', $ingredient->id)->firstOrFail();
                    if ($inventory->quantity < $neededAmount) {
                        throw new \Exception("Not enough stock for " . $ingredient->name);
                    }

                    $inventory->decrement('quantity', $neededAmount);

                    StockMovement::create([
                        'ingredient_id' => $ingredient->id,
                        'change_amount' => -$neededAmount,
                        'reason' => 'sale'
                    ]);
                }
            }

            DB::commit();

            return response()->json([
                'success' => true,
                'message' => 'Order created successfully',
                'data' => $order->load('items.product')
            ], 201);

        } catch (\Exception $e) {

            DB::rollBack();

            return response()->json([
                'success' => false,
                'message' => $e->getMessage()
            ], 400);
        }
    }

    public function updateStatus(Request $request, $id)
    {
        $request->validate([
            'status' => 'required|string'
        ]);

        $order = Order::findOrFail($id);
        $order->status = $request->status;
        $order->save();

        return response()->json([
            'success' => true,
            'message' => 'Status updated',
            'data' => $order
        ]);
    }

    public function getByDate($date)
    {
        return Order::with('items.product')
            ->whereDate('created_at', $date)
            ->get();
    }
}
