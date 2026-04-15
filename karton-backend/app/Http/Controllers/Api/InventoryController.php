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
        // return plain array so clients expecting a JSON array can deserialize directly
        return response()->json($inventory);
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

        return response()->json($inventory->load('ingredient'));
    }

    public function lowStock()
    {
        $items = Inventory::with('ingredient')
            ->whereColumn('quantity', '<', 'minimum_level')
            ->get();

        // return plain array for consistency with index()
        return response()->json($items);
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

        return response()->json($inventory->load('ingredient'));
    }
}
