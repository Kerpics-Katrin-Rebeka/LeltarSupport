<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Models\PurchaseOrder;
use App\Models\PurchaseOrderItem;

class PurchaseOrderController extends Controller
{
    public function index()
    {
        return PurchaseOrder::with('items.ingredient')->get();
    }

    public function store(Request $request)
    {
        $request->validate([
            'supplier_id'=>'required|exists:suppliers,id',
            'status'=>'required|string',
            'items'=>'required|array'
        ]);

        $po = PurchaseOrder::create([
            'supplier_id'=>$request->supplier_id,
            'status'=>$request->status
        ]);

        foreach($request->items as $item){
            PurchaseOrderItem::create([
                'purchase_order_id'=>$po->id,
                'ingredient_id'=>$item['ingredient_id'],
                'quantity'=>$item['quantity']
            ]);
        }

        return response()->json($po->load('items.ingredient'),201);
    }

    public function show($id)
    {
        return PurchaseOrder::with('items.ingredient')->findOrFail($id);
    }
}
