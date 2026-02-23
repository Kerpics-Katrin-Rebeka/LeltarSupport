<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Models\Product;
use App\Models\Ingredient;

class ProductController extends Controller
{
    public function index()
    {
        return Product::with('ingredients')->get();
    }

    public function store(Request $request)
    {
        $request->validate([
            'name'=>'required|string',
            'price'=>'required|numeric',
            'active'=>'boolean',
            'ingredients'=>'array'
        ]);

        $product = Product::create($request->only('name','price','active'));

        if($request->has('ingredients')){
            foreach($request->ingredients as $ing){
                $product->ingredients()->attach($ing['id'], ['quantity'=>$ing['quantity']]);
            }
        }

        return response()->json($product->load('ingredients'),201);
    }

    public function show($id)
    {
        return Product::with('ingredients')->findOrFail($id);
    }

    public function update(Request $request, $id)
    {
        $product = Product::findOrFail($id);
        $product->update($request->only('name','price','active'));

        if($request->has('ingredients')){
            $product->ingredients()->sync([]);
            foreach($request->ingredients as $ing){
                $product->ingredients()->attach($ing['id'], ['quantity'=>$ing['quantity']]);
            }
        }

        return response()->json($product->load('ingredients'));
    }

    public function destroy($id)
    {
        $product = Product::findOrFail($id);
        $product->ingredients()->detach();
        $product->delete();
        return response()->json(['message'=>'Deleted']);
    }
}
