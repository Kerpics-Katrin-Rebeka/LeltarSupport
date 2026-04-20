<?php

namespace Tests\Feature;

use App\Models\Ingredient;
use App\Models\Inventory;
use App\Models\Product;
use App\Models\Supplier;
use App\Models\User;
use Illuminate\Foundation\Testing\RefreshDatabase;
use Illuminate\Support\Facades\Hash;
use Laravel\Sanctum\Sanctum;
use Tests\TestCase;

abstract class ApiTestCase extends TestCase
{
    use RefreshDatabase;

    protected function createUser(array $attributes = []): User
    {
        return User::create(array_merge([
            'name' => 'Test User',
            'email' => 'user'.uniqid().'@test.com',
            'password' => Hash::make('password123'),
        ], $attributes));
    }

    protected function authenticate(?User $user = null): User
    {
        $user ??= $this->createUser();
        Sanctum::actingAs($user);

        return $user;
    }

    protected function createIngredientWithInventory(
        string $name = 'Ingredient',
        string $unit = 'kg',
        float $quantity = 10,
        float $minimumLevel = 2
    ): Ingredient {
        $ingredient = Ingredient::create([
            'name' => $name.uniqid(),
            'unit' => $unit,
        ]);

        Inventory::create([
            'ingredient_id' => $ingredient->id,
            'quantity' => $quantity,
            'minimum_level' => $minimumLevel,
        ]);

        return $ingredient->fresh();
    }

    protected function createProductWithIngredients(array $ingredientDefinitions, array $attributes = []): Product
    {
        $product = Product::create(array_merge([
            'name' => 'Product '.uniqid(),
            'price' => 1990,
            'active' => true,
        ], $attributes));

        foreach ($ingredientDefinitions as $definition) {
            $product->ingredients()->attach($definition['ingredient']->id, [
                'quantity' => $definition['quantity'],
            ]);
        }

        return $product->fresh()->load('ingredients');
    }

    protected function createSupplier(array $attributes = []): Supplier
    {
        return Supplier::create(array_merge([
            'name' => 'Supplier '.uniqid(),
        ], $attributes));
    }
}
