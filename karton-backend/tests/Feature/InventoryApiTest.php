<?php

namespace Tests\Feature;

use App\Models\StockMovement;

class InventoryApiTest extends ApiTestCase
{
    public function test_authenticated_user_can_list_inventory_with_ingredients(): void
    {
        $this->authenticate();
        $ingredient = $this->createIngredientWithInventory('Flour', 'kg', 12, 3);

        $response = $this->getJson('/api/inventory');

        $response
            ->assertOk()
            ->assertJsonPath('success', true)
            ->assertJsonPath('data.0.ingredient.id', $ingredient->id);
    }

    public function test_authenticated_user_can_update_inventory_quantity_and_stock_movement_is_created(): void
    {
        $this->authenticate();
        $ingredient = $this->createIngredientWithInventory('Oil', 'l', 10, 2);

        $response = $this->putJson('/api/inventory/'.$ingredient->id, [
            'quantity' => 16,
        ]);

        $response
            ->assertOk()
            ->assertJsonPath('success', true)
            ->assertJsonPath('data.quantity', 16);

        $this->assertDatabaseHas('inventories', [
            'ingredient_id' => $ingredient->id,
            'quantity' => 16,
        ]);

        $this->assertDatabaseHas('stock_movements', [
            'ingredient_id' => $ingredient->id,
            'change_amount' => 6,
            'reason' => 'manual_adjustment',
        ]);
    }

    public function test_update_inventory_validates_quantity(): void
    {
        $this->authenticate();
        $ingredient = $this->createIngredientWithInventory('Rice', 'kg', 10, 2);

        $response = $this->putJson('/api/inventory/'.$ingredient->id, [
            'quantity' => -1,
        ]);

        $response
            ->assertStatus(422)
            ->assertJsonValidationErrors(['quantity']);
    }

    public function test_authenticated_user_can_list_low_stock_items(): void
    {
        $this->authenticate();
        $low = $this->createIngredientWithInventory('Low', 'kg', 1, 5);
        $this->createIngredientWithInventory('Enough', 'kg', 10, 5);

        $response = $this->getJson('/api/inventory/low-stock');

        $response
            ->assertOk()
            ->assertJsonPath('success', true)
            ->assertJsonCount(1, 'data')
            ->assertJsonPath('data.0.ingredient_id', $low->id);
    }

    public function test_authenticated_user_can_restock_inventory_and_stock_movement_is_created(): void
    {
        $this->authenticate();
        $ingredient = $this->createIngredientWithInventory('Sugar', 'kg', 5, 2);

        $response = $this->postJson('/api/inventory/restock', [
            'ingredient_id' => $ingredient->id,
            'quantity' => 4,
        ]);

        $response
            ->assertOk()
            ->assertJsonPath('success', true)
            ->assertJsonPath('message', 'Stock increased');

        $this->assertDatabaseHas('inventories', [
            'ingredient_id' => $ingredient->id,
            'quantity' => 9,
        ]);

        $this->assertDatabaseHas('stock_movements', [
            'ingredient_id' => $ingredient->id,
            'change_amount' => 4,
            'reason' => 'restock',
        ]);
    }

    public function test_restock_validates_payload(): void
    {
        $this->authenticate();

        $response = $this->postJson('/api/inventory/restock', [
            'ingredient_id' => 999,
            'quantity' => 0,
        ]);

        $response
            ->assertStatus(422)
            ->assertJsonValidationErrors(['ingredient_id', 'quantity']);
    }

    public function test_inventory_routes_require_authentication(): void
    {
        $this->getJson('/api/inventory')->assertStatus(401);
        $this->putJson('/api/inventory/1', [])->assertStatus(401);
        $this->getJson('/api/inventory/low-stock')->assertStatus(401);
        $this->postJson('/api/inventory/restock', [])->assertStatus(401);
    }
}
