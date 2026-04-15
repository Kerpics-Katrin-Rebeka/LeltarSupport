<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Models\Supplier;

class SupplierController extends Controller
{
   public function index()
    {
        $suppliers = Supplier::all();

        return response()->json([
            'success' => true,
            'data' => $suppliers
        ]);
    }

    public function store(Request $request)
    {
        $request->validate([
            'name' => 'required|string|unique:suppliers,name',
            'email' => 'nullable|email',
            'phone' => 'nullable|string'
        ]);

        $supplier = Supplier::create([
            'name' => $request->name,
            'email' => $request->email,
            'phone' => $request->phone
        ]);

        return response()->json([
            'success' => true,
            'message' => 'Supplier created',
            'data' => $supplier
        ], 201);
    }

    public function show($id)
    {
        $supplier = Supplier::with('purchaseOrders')->findOrFail($id);

        return response()->json([
            'success' => true,
            'data' => $supplier
        ]);
    }

 public function update(Request $request, $id)
    {
        $supplier = Supplier::findOrFail($id);

        $request->validate([
            'name' => 'required|string|unique:suppliers,name,' . $id,
            'email' => 'nullable|email',
            'phone' => 'nullable|string'
        ]);

        $supplier->update([
            'name' => $request->name,
            'email' => $request->email,
            'phone' => $request->phone
        ]);

        return response()->json([
            'success' => true,
            'message' => 'Supplier updated',
            'data' => $supplier
        ]);
    }

    public function destroy($id)
    {
        $supplier = Supplier::findOrFail($id);

        if ($supplier->purchaseOrders()->exists()) {
            return response()->json([
                'success' => false,
                'message' => 'Supplier has purchase orders and cannot be deleted'
            ], 400);
        }

        $supplier->delete();

        return response()->json([
            'success' => true,
            'message' => 'Supplier deleted'
        ]);
    }
}
