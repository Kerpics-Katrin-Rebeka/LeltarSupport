<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Models\Ingredient;

class IngredientController extends Controller
{
    public function index()
    {
        $ingredients = Ingredient::all();

        return response()->json([
            'success' => true,
            'data' => $ingredients
        ]);
    }

    public function store(Request $request)
    {
        $request->validate([
            'name' => 'required|string|unique:ingredients,name',
            'unit' => 'required|string'
        ]);

        $ingredient = Ingredient::create([
            'name' => $request->name,
            'unit' => $request->unit
        ]);

        return response()->json([
            'success' => true,
            'message' => 'Ingredient created',
            'data' => $ingredient
        ], 201);
    }

    public function show($id)
    {
        $ingredient = Ingredient::findOrFail($id);

        return response()->json([
            'success' => true,
            'data' => $ingredient
        ]);
    }

    public function update(Request $request, $id)
    {
        $ingredient = Ingredient::findOrFail($id);

        $request->validate([
            'name' => 'required|string|unique:ingredients,name,' . $id,
            'unit' => 'required|string'
        ]);

        $ingredient->update([
            'name' => $request->name,
            'unit' => $request->unit
        ]);

        return response()->json([
            'success' => true,
            'message' => 'Ingredient updated',
            'data' => $ingredient
        ]);
    }
    
    public function destroy($id)
    {
        $ingredient = Ingredient::findOrFail($id);

        $ingredient->delete();

        return response()->json([
            'success' => true,
            'message' => 'Ingredient deleted'
        ]);
    }
}
