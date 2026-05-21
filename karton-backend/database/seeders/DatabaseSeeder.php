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
use App\Models\Role;

class DatabaseSeeder extends Seeder
{
    public function run(): void
    {
        // roles
        $adminRole = Role::firstOrCreate(['name' => 'admin']);
        $managerRole = Role::firstOrCreate(['name' => 'manager']);
        $staffRole = Role::firstOrCreate(['name' => 'staff']);

        // users
        $admin = User::updateOrCreate(
            ['email' => 'admin@test.com'],
            ['name' => 'Admin', 'password' => Hash::make('123456')]
        );

        $manager = User::updateOrCreate(
            ['email' => 'manager@test.com'],
            ['name' => 'Manager', 'password' => Hash::make('123456')]
        );

        $staff = User::updateOrCreate(
            ['email' => 'staff@test.com'],
            ['name' => 'Staff', 'password' => Hash::make('123456')]
        );

        $admin->roles()->sync([$adminRole->id]);
        $manager->roles()->sync([$managerRole->id]);
        $staff->roles()->sync([$staffRole->id]);

        // ingredients
        $bun = Ingredient::firstOrCreate(['name' => 'Bun'], ['unit' => 'db']);
        $meat = Ingredient::firstOrCreate(['name' => 'Meat'], ['unit' => 'kg']);
        $cheese = Ingredient::firstOrCreate(['name' => 'Cheese'], ['unit' => 'kg']);
        $lettuce = Ingredient::firstOrCreate(['name' => 'Lettuce'], ['unit' => 'kg']);
        $tomato = Ingredient::firstOrCreate(['name' => 'Tomato'], ['unit' => 'kg']);
        $onion = Ingredient::firstOrCreate(['name' => 'Onion'], ['unit' => 'kg']);
        $potato = Ingredient::firstOrCreate(['name' => 'Potato'], ['unit' => 'kg']);
        $oil = Ingredient::firstOrCreate(['name' => 'Oil'], ['unit' => 'l']);
        $colaSyrup = Ingredient::firstOrCreate(['name' => 'Cola Syrup'], ['unit' => 'l']);
        $cup = Ingredient::firstOrCreate(['name' => 'Cup'], ['unit' => 'db']);

        // inventory
        Inventory::updateOrCreate(['ingredient_id' => $bun->id], ['quantity' => 120, 'minimum_level' => 20]);
        Inventory::updateOrCreate(['ingredient_id' => $meat->id], ['quantity' => 65, 'minimum_level' => 12]);
        Inventory::updateOrCreate(['ingredient_id' => $cheese->id], ['quantity' => 40, 'minimum_level' => 8]);
        Inventory::updateOrCreate(['ingredient_id' => $lettuce->id], ['quantity' => 18, 'minimum_level' => 5]);
        Inventory::updateOrCreate(['ingredient_id' => $tomato->id], ['quantity' => 22, 'minimum_level' => 6]);
        Inventory::updateOrCreate(['ingredient_id' => $onion->id], ['quantity' => 15, 'minimum_level' => 5]);
        Inventory::updateOrCreate(['ingredient_id' => $potato->id], ['quantity' => 80, 'minimum_level' => 20]);
        Inventory::updateOrCreate(['ingredient_id' => $oil->id], ['quantity' => 30, 'minimum_level' => 8]);
        Inventory::updateOrCreate(['ingredient_id' => $colaSyrup->id], ['quantity' => 12, 'minimum_level' => 3]);
        Inventory::updateOrCreate(['ingredient_id' => $cup->id], ['quantity' => 250, 'minimum_level' => 50]);

        // products
        $cheeseburger = Product::updateOrCreate(
            ['name' => 'Cheeseburger'],
            ['price' => 1500, 'active' => true]
        );

        $chickenBurger = Product::updateOrCreate(
            ['name' => 'Chicken Burger'],
            ['price' => 1650, 'active' => true]
        );

        $veggieBurger = Product::updateOrCreate(
            ['name' => 'Veggie Burger'],
            ['price' => 1450, 'active' => true]
        );

        $fries = Product::updateOrCreate(
            ['name' => 'French Fries'],
            ['price' => 750, 'active' => true]
        );

        $cola = Product::updateOrCreate(
            ['name' => 'Cola'],
            ['price' => 650, 'active' => true]
        );

        // product ingredients
        $cheeseburger->ingredients()->sync([
            $bun->id => ['quantity' => 1],
            $meat->id => ['quantity' => 0.20],
            $cheese->id => ['quantity' => 0.05],
            $lettuce->id => ['quantity' => 0.02],
            $tomato->id => ['quantity' => 0.03],
            $onion->id => ['quantity' => 0.02],
        ]);

        $chickenBurger->ingredients()->sync([
            $bun->id => ['quantity' => 1],
            $meat->id => ['quantity' => 0.18],
            $lettuce->id => ['quantity' => 0.03],
            $tomato->id => ['quantity' => 0.02],
        ]);

        $veggieBurger->ingredients()->sync([
            $bun->id => ['quantity' => 1],
            $cheese->id => ['quantity' => 0.03],
            $lettuce->id => ['quantity' => 0.04],
            $tomato->id => ['quantity' => 0.04],
            $onion->id => ['quantity' => 0.02],
        ]);

        $fries->ingredients()->sync([
            $potato->id => ['quantity' => 0.25],
            $oil->id => ['quantity' => 0.02],
        ]);

        $cola->ingredients()->sync([
            $colaSyrup->id => ['quantity' => 0.03],
            $cup->id => ['quantity' => 1],
        ]);

        // suppliers
        $supplier1 = Supplier::firstOrNew(['name' => 'Food Supplier Ltd.']);
        $supplier1->email = 'supplier@test.com';
        $supplier1->phone = '123456789';
        $supplier1->save();

        $supplier2 = Supplier::firstOrNew(['name' => 'Fresh Farm Partners']);
        $supplier2->email = 'freshfarm@test.com';
        $supplier2->phone = '06701112233';
        $supplier2->save();

        $supplier3 = Supplier::firstOrNew(['name' => 'City Beverage Co.']);
        $supplier3->email = 'beverage@test.com';
        $supplier3->phone = '06704445566';
        $supplier3->save();

        // purchase orders
        $po1 = PurchaseOrder::firstOrCreate([
            'supplier_id' => $supplier1->id,
            'status' => 'pending',
        ]);

        $po2 = PurchaseOrder::firstOrCreate([
            'supplier_id' => $supplier2->id,
            'status' => 'received',
        ]);

        $po3 = PurchaseOrder::firstOrCreate([
            'supplier_id' => $supplier3->id,
            'status' => 'pending',
        ]);

        PurchaseOrderItem::firstOrCreate([
            'purchase_order_id' => $po1->id,
            'ingredient_id' => $bun->id,
            'quantity' => 60,
        ]);

        PurchaseOrderItem::firstOrCreate([
            'purchase_order_id' => $po1->id,
            'ingredient_id' => $meat->id,
            'quantity' => 30,
        ]);

        PurchaseOrderItem::firstOrCreate([
            'purchase_order_id' => $po2->id,
            'ingredient_id' => $potato->id,
            'quantity' => 120,
        ]);

        PurchaseOrderItem::firstOrCreate([
            'purchase_order_id' => $po2->id,
            'ingredient_id' => $oil->id,
            'quantity' => 20,
        ]);

        PurchaseOrderItem::firstOrCreate([
            'purchase_order_id' => $po3->id,
            'ingredient_id' => $colaSyrup->id,
            'quantity' => 25,
        ]);

        PurchaseOrderItem::firstOrCreate([
            'purchase_order_id' => $po3->id,
            'ingredient_id' => $cup->id,
            'quantity' => 300,
        ]);
    }
}
