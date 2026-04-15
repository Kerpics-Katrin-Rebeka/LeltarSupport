<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Models\Inventory;
use App\Models\StockMovement;

class InventoryController extends Controller
{
    public function index()
    {
        $inventory = Inventory::with('ingredient')->get();

        return response()->json([
            'success' => true,
            'data' => $inventory
        ]);
    }

    public function update(Request $request, $ingredient_id)
    {
        $request->validate([
            'quantity' => 'required|numeric|min:0'
        ]);

        $inventory = Inventory::where('ingredient_id', $ingredient_id)->firstOrFail();

        $difference = $request->quantity - $inventory->quantity;

        $inventory->update([
            'quantity' => $request->quantity
        ]);

        StockMovement::create([
            'ingredient_id' => $ingredient_id,
            'change_amount' => $difference,
            'reason' => 'manual_adjustment'
        ]);

        return response()->json([
            'success' => true,
            'message' => 'Stock updated',
            'data' => $inventory->load('ingredient')
        ]);
    }

    public function lowStock()
    {
        $items = Inventory::with('ingredient')
            ->whereColumn('quantity', '<', 'minimum_level')
            ->get();

        return response()->json([
            'success' => true,
            'data' => $items
        ]);
    }

    public function restock(Request $request)
    {
        $request->validate([
            'ingredient_id' => 'required|exists:ingredients,id',
            'quantity' => 'required|numeric|min:1'
        ]);

        $inventory = Inventory::where('ingredient_id', $request->ingredient_id)->firstOrFail();

        $inventory->increment('quantity', $request->quantity);

        StockMovement::create([
            'ingredient_id' => $request->ingredient_id,
            'change_amount' => $request->quantity,
            'reason' => 'restock'
        ]);

        return response()->json([
            'success' => true,
            'message' => 'Stock increased',
            'data' => $inventory->load('ingredient')
        ]);
    }
    
    public function stockMovements()
    {
        return StockMovement::with('ingredient')->orderBy('created_at', 'desc')->get();
    }
}
