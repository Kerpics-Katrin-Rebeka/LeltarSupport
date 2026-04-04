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
        return Inventory::with('ingredient')->get();
    }

    public function update(Request $request, $ingredient_id)
    {
        $request->validate([
            'quantity'=>'required|numeric'
        ]);

        $inventory = Inventory::where('ingredient_id', $ingredient_id)->firstOrFail();
        $change = $request->quantity - $inventory->quantity;
        $inventory->quantity = $request->quantity;
        $inventory->save();

        StockMovement::create([
            'ingredient_id'=>$ingredient_id,
            'change_amount'=>$change,
            'reason'=>'manual'
        ]);

        return response()->json($inventory->load('ingredient'));
    }

    public function stockMovements()
    {
        return StockMovement::with('ingredient')->orderBy('created_at', 'desc')->get();
    }
}
