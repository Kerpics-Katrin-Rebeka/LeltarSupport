<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Models\Product;

class ProductController extends Controller
{
    public function index()
    {
        $products = Product::with('ingredients')->get();

        return response()->json([
            'success' => true,
            'data' => $products
        ]);
    }

    public function store(Request $request)
    {
        $request->validate([
            'name' => 'required|string|unique:products,name',
            'price' => 'required|numeric|min:0',
            'active' => 'boolean',
            'ingredients' => 'required|array|min:1',
            'ingredients.*.id' => 'required|exists:ingredients,id',
            'ingredients.*.quantity' => 'required|numeric|min:0.001'
        ]);

        $product = Product::create([
            'name' => $request->name,
            'price' => $request->price,
            'active' => $request->active ?? true
        ]);

        foreach ($request->ingredients as $ingredient) {
            $product->ingredients()->attach($ingredient['id'], [
                'quantity' => $ingredient['quantity']
            ]);
        }

        return response()->json([
            'success' => true,
            'message' => 'Product created',
            'data' => $product->load('ingredients')
        ], 201);
    }


    public function show($id)
    {
        $product = Product::with('ingredients')->findOrFail($id);

        return response()->json([
            'success' => true,
            'data' => $product
        ]);
    }

    public function update(Request $request, $id)
    {
        $product = Product::findOrFail($id);

        $request->validate([
            'name' => 'required|string|unique:products,name,' . $id,
            'price' => 'required|numeric|min:0',
            'active' => 'boolean',
            'ingredients' => 'array',
            'ingredients.*.id' => 'exists:ingredients,id',
            'ingredients.*.quantity' => 'numeric|min:0.001'
        ]);

        $product->update([
            'name' => $request->name,
            'price' => $request->price,
            'active' => $request->active ?? $product->active
        ]);

        if ($request->has('ingredients')) {
            $syncData = [];

            foreach ($request->ingredients as $ingredient) {
                $syncData[$ingredient['id']] = [
                    'quantity' => $ingredient['quantity']
                ];
            }

            $product->ingredients()->sync($syncData);
        }

        return response()->json([
            'success' => true,
            'message' => 'Product updated',
            'data' => $product->load('ingredients')
        ]);
    }

    public function destroy($id)
    {
        $product = Product::findOrFail($id);

        $product->ingredients()->detach();

        $product->delete();

        return response()->json([
            'success' => true,
            'message' => 'Product deleted'
        ]);
    }
}
