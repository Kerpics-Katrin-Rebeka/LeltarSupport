<?php

namespace Database\Seeders;

use Illuminate\Database\Seeder;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Facades\Hash;
use Illuminate\Support\Facades\Schema;

class InventorySqlSeeder extends Seeder
{
    /**
     * Seed the database created by invenotry_database.sql.
     */
    public function run(): void
    {
        $tables = [
            'roles',
            'users',
            'user_roles',
            'products',
            'ingredients',
            'product_ingredients',
            'inventory',
            'stock_movements',
            'orders',
            'order_items',
            'suppliers',
            'purchase_orders',
            'purchase_order_items',
        ];

        foreach ($tables as $table) {
            if (!Schema::hasTable($table)) {
                throw new \RuntimeException("Table '{$table}' was not found. Run invenotry_database.sql first.");
            }
        }

        DB::transaction(function () {
            DB::table('purchase_order_items')->delete();
            DB::table('purchase_orders')->delete();
            DB::table('order_items')->delete();
            DB::table('orders')->delete();
            DB::table('stock_movements')->delete();
            DB::table('inventory')->delete();
            DB::table('product_ingredients')->delete();
            DB::table('user_roles')->delete();
            DB::table('suppliers')->delete();
            DB::table('products')->delete();
            DB::table('ingredients')->delete();
            DB::table('users')->delete();
            DB::table('roles')->delete();

            DB::table('roles')->insert([
                ['id' => 1, 'name' => 'admin'],
                ['id' => 2, 'name' => 'manager'],
                ['id' => 3, 'name' => 'staff'],
            ]);

            DB::table('users')->insert([
                [
                    'id' => 1,
                    'name' => 'Admin User',
                    'email' => 'admin@inventory.local',
                    'password_hash' => Hash::make('Admin123!'),
                    'created_at' => now(),
                ],
                [
                    'id' => 2,
                    'name' => 'Manager User',
                    'email' => 'manager@inventory.local',
                    'password_hash' => Hash::make('Manager123!'),
                    'created_at' => now(),
                ],
                [
                    'id' => 3,
                    'name' => 'Staff User',
                    'email' => 'staff@inventory.local',
                    'password_hash' => Hash::make('Staff123!'),
                    'created_at' => now(),
                ],
            ]);

            DB::table('user_roles')->insert([
                ['user_id' => 1, 'role_id' => 1],
                ['user_id' => 2, 'role_id' => 2],
                ['user_id' => 3, 'role_id' => 3],
            ]);

            DB::table('suppliers')->insert([
                ['id' => 1, 'name' => 'Fresh Farm Kft', 'contact' => 'fresh@farm.hu'],
                ['id' => 2, 'name' => 'Daily Dairy Bt', 'contact' => 'sales@dailydairy.hu'],
            ]);

            DB::table('ingredients')->insert([
                ['id' => 1, 'name' => 'Flour', 'unit' => 'kg'],
                ['id' => 2, 'name' => 'Milk', 'unit' => 'l'],
                ['id' => 3, 'name' => 'Sugar', 'unit' => 'kg'],
                ['id' => 4, 'name' => 'Egg', 'unit' => 'pcs'],
            ]);

            DB::table('inventory')->insert([
                ['ingredient_id' => 1, 'quantity' => 30.00, 'minimum_level' => 10.00],
                ['ingredient_id' => 2, 'quantity' => 20.00, 'minimum_level' => 8.00],
                ['ingredient_id' => 3, 'quantity' => 12.00, 'minimum_level' => 5.00],
                ['ingredient_id' => 4, 'quantity' => 120.00, 'minimum_level' => 50.00],
            ]);

            DB::table('products')->insert([
                ['id' => 1, 'name' => 'Pancake', 'price' => 1500.00, 'active' => 1],
                ['id' => 2, 'name' => 'Waffle', 'price' => 1800.00, 'active' => 1],
                ['id' => 3, 'name' => 'Crepe', 'price' => 1700.00, 'active' => 1],
            ]);

            DB::table('product_ingredients')->insert([
                ['product_id' => 1, 'ingredient_id' => 1, 'quantity' => 0.20],
                ['product_id' => 1, 'ingredient_id' => 2, 'quantity' => 0.10],
                ['product_id' => 1, 'ingredient_id' => 4, 'quantity' => 2.00],
                ['product_id' => 2, 'ingredient_id' => 1, 'quantity' => 0.25],
                ['product_id' => 2, 'ingredient_id' => 2, 'quantity' => 0.12],
                ['product_id' => 2, 'ingredient_id' => 4, 'quantity' => 2.00],
                ['product_id' => 3, 'ingredient_id' => 1, 'quantity' => 0.18],
                ['product_id' => 3, 'ingredient_id' => 2, 'quantity' => 0.10],
                ['product_id' => 3, 'ingredient_id' => 3, 'quantity' => 0.03],
                ['product_id' => 3, 'ingredient_id' => 4, 'quantity' => 1.00],
            ]);

            DB::table('stock_movements')->insert([
                ['ingredient_id' => 1, 'change_amount' => 30.00, 'reason' => 'restock', 'created_at' => now()],
                ['ingredient_id' => 2, 'change_amount' => 20.00, 'reason' => 'restock', 'created_at' => now()],
                ['ingredient_id' => 3, 'change_amount' => 12.00, 'reason' => 'restock', 'created_at' => now()],
                ['ingredient_id' => 4, 'change_amount' => 120.00, 'reason' => 'restock', 'created_at' => now()],
            ]);

            DB::table('orders')->insert([
                ['id' => 1, 'user_id' => 3, 'total_price' => 3300.00, 'created_at' => now()],
            ]);

            DB::table('order_items')->insert([
                ['order_id' => 1, 'product_id' => 1, 'quantity' => 1],
                ['order_id' => 1, 'product_id' => 2, 'quantity' => 1],
            ]);

            DB::table('purchase_orders')->insert([
                ['id' => 1, 'supplier_id' => 1, 'status' => 'recommended', 'created_at' => now()],
            ]);

            DB::table('purchase_order_items')->insert([
                ['purchase_order_id' => 1, 'ingredient_id' => 1, 'quantity' => 25.00],
                ['purchase_order_id' => 1, 'ingredient_id' => 2, 'quantity' => 15.00],
            ]);
        });
    }
}
