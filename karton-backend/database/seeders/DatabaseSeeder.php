<?php

namespace Database\Seeders;

use Illuminate\Database\Seeder;
use Illuminate\Support\Facades\Hash;

use App\Models\User;
use App\Models\Ingredient;
use App\Models\Inventory;
use App\Models\Product;
use App\Models\Supplier;
use App\Models\PurchaseOrder;
use App\Models\PurchaseOrderItem;

class DatabaseSeeder extends Seeder
{
    public function run(): void
    {
        // admin user
        $user = User::create([
            'name' => 'Admin',
            'email' => 'admin@test.com',
            'password' => Hash::make('123456')
        ]);

        // ingredients
        $bun = Ingredient::create(['name' => 'Bun', 'unit' => 'db']);
        $meat = Ingredient::create(['name' => 'Meat', 'unit' => 'kg']);
        $cheese = Ingredient::create(['name' => 'Cheese', 'unit' => 'kg']);

        // inventory
        Inventory::create([
            'ingredient_id' => $bun->id,
            'quantity' => 100,
            'minimum_level' => 20
        ]);

        Inventory::create([
            'ingredient_id' => $meat->id,
            'quantity' => 50,
            'minimum_level' => 10
        ]);

        Inventory::create([
            'ingredient_id' => $cheese->id,
            'quantity' => 30,
            'minimum_level' => 5
        ]);

        // product
        $burger = Product::create([
            'name' => 'Cheeseburger',
            'price' => 1500,
            'active' => true
        ]);

        // prod ingredients
        $burger->ingredients()->attach($bun->id, ['quantity' => 1]);
        $burger->ingredients()->attach($meat->id, ['quantity' => 0.2]);
        $burger->ingredients()->attach($cheese->id, ['quantity' => 0.05]);

        // suppliers
        $supplier = Supplier::create([
            'name' => 'Food Supplier Ltd.',
            'email' => 'supplier@test.com',
            'phone' => '123456789'
        ]);

        // orders
        $po = PurchaseOrder::create([
            'supplier_id' => $supplier->id,
            'status' => 'pending'
        ]);

        PurchaseOrderItem::create([
            'purchase_order_id' => $po->id,
            'ingredient_id' => $bun->id,
            'quantity' => 50
        ]);

        PurchaseOrderItem::create([
            'purchase_order_id' => $po->id,
            'ingredient_id' => $meat->id,
            'quantity' => 20
        ]);
    }
}