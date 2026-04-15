<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Models\PurchaseOrder;
use App\Models\PurchaseOrderItem;
use App\Models\Inventory;
use App\Models\StockMovement;
use Illuminate\Support\Facades\DB;

class PurchaseOrderController extends Controller
{
    public function index()
    {
        $orders = PurchaseOrder::with('items.ingredient', 'supplier')
            ->latest()
            ->get();

        return response()->json($orders);
    }

    public function show($id)
    {
        $order = PurchaseOrder::with('items.ingredient', 'supplier')
            ->findOrFail($id);

        return response()->json($order);
    }

    public function store(Request $request)
    {
        $request->validate([
            'supplier_id' => 'required|exists:suppliers,id',
            'items' => 'required|array|min:1',
            'items.*.ingredient_id' => 'required|exists:ingredients,id',
            'items.*.quantity' => 'required|numeric|min:1'
        ]);

        DB::beginTransaction();

        try {
            $order = PurchaseOrder::create([
                'supplier_id' => $request->supplier_id,
                'status' => 'pending'
            ]);

            foreach ($request->items as $item) {
                PurchaseOrderItem::create([
                    'purchase_order_id' => $order->id,
                    'ingredient_id' => $item['ingredient_id'],
                    'quantity' => $item['quantity']
                ]);
            }

            DB::commit();

            return response()->json($order->load('items.ingredient'), 201);

        } catch (\Exception $e) {
            DB::rollBack();

            return response()->json([
                'message' => $e->getMessage()
            ], 400);
        }
    }

    public function receive($id)
    {
        $order = PurchaseOrder::with('items')->findOrFail($id);

        if ($order->status === 'received') {
            return response()->json([
                'message' => 'Already received'
            ], 400);
        }

        DB::beginTransaction();

        try {
            foreach ($order->items as $item) {

                $inventory = Inventory::where('ingredient_id', $item->ingredient_id)->firstOrFail();

                $inventory->increment('quantity', $item->quantity);

                StockMovement::create([
                    'ingredient_id' => $item->ingredient_id,
                    'change_amount' => $item->quantity,
                    'reason' => 'purchase'
                ]);
            }

            $order->status = 'received';
            $order->save();

            DB::commit();

            return response()->json($order->load('items.ingredient'));

        } catch (\Exception $e) {
            DB::rollBack();

            return response()->json([
                'message' => $e->getMessage()
            ], 400);
        }
    }
}
