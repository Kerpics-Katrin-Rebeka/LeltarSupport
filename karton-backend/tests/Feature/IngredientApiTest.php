<?php

namespace Tests\Feature;

use App\Models\Ingredient;

class IngredientApiTest extends ApiTestCase
{
    public function test_public_test_ingredients_endpoint_returns_ingredients(): void
    {
        Ingredient::create(['name' => 'Flour', 'unit' => 'kg']);

        $response = $this->getJson('/api/test-ingredients');

        $response
            ->assertOk()
            ->assertJsonPath('success', true)
            ->assertJsonCount(1, 'data');
    }

    public function test_authenticated_user_can_list_ingredients(): void
    {
        $this->authenticate();
        Ingredient::create(['name' => 'Tomato', 'unit' => 'kg']);
        Ingredient::create(['name' => 'Salt', 'unit' => 'g']);

        $response = $this->getJson('/api/ingredients');

        $response
            ->assertOk()
            ->assertJsonPath('success', true)
            ->assertJsonCount(2, 'data');
    }

    public function test_authenticated_user_can_create_ingredient(): void
    {
        $this->authenticate();

        $response = $this->postJson('/api/ingredients', [
            'name' => 'Onion',
            'unit' => 'kg',
        ]);

        $response
            ->assertStatus(201)
            ->assertJsonPath('success', true)
            ->assertJsonPath('data.name', 'Onion')
            ->assertJsonPath('data.unit', 'kg');

        $this->assertDatabaseHas('ingredients', [
            'name' => 'Onion',
            'unit' => 'kg',
        ]);
    }

    public function test_create_ingredient_validates_required_fields_and_uniqueness(): void
    {
        $this->authenticate();
        Ingredient::create(['name' => 'Sugar', 'unit' => 'kg']);

        $response = $this->postJson('/api/ingredients', [
            'name' => 'Sugar',
        ]);

        $response
            ->assertStatus(422)
            ->assertJsonValidationErrors(['name', 'unit']);
    }

    public function test_authenticated_user_can_show_ingredient(): void
    {
        $this->authenticate();
        $ingredient = Ingredient::create(['name' => 'Paprika', 'unit' => 'g']);

        $response = $this->getJson('/api/ingredients/'.$ingredient->id);

        $response
            ->assertOk()
            ->assertJsonPath('data.id', $ingredient->id);
    }

    public function test_authenticated_user_can_update_ingredient(): void
    {
        $this->authenticate();
        $ingredient = Ingredient::create(['name' => 'Milk', 'unit' => 'l']);

        $response = $this->putJson('/api/ingredients/'.$ingredient->id, [
            'name' => 'Oat Milk',
            'unit' => 'l',
        ]);

        $response
            ->assertOk()
            ->assertJsonPath('data.name', 'Oat Milk');

        $this->assertDatabaseHas('ingredients', [
            'id' => $ingredient->id,
            'name' => 'Oat Milk',
        ]);
    }

    public function test_update_ingredient_validates_unique_name_except_current_record(): void
    {
        $this->authenticate();
        $ingredientA = Ingredient::create(['name' => 'Cheese', 'unit' => 'kg']);
        $ingredientB = Ingredient::create(['name' => 'Ham', 'unit' => 'kg']);

        $response = $this->putJson('/api/ingredients/'.$ingredientB->id, [
            'name' => 'Cheese',
            'unit' => 'kg',
        ]);

        $response
            ->assertStatus(422)
            ->assertJsonValidationErrors(['name']);
    }

    public function test_authenticated_user_can_delete_ingredient(): void
    {
        $this->authenticate();
        $ingredient = Ingredient::create(['name' => 'Lettuce', 'unit' => 'db']);

        $response = $this->deleteJson('/api/ingredients/'.$ingredient->id);

        $response
            ->assertOk()
            ->assertJsonPath('success', true);

        $this->assertDatabaseMissing('ingredients', [
            'id' => $ingredient->id,
        ]);
    }

    public function test_ingredient_routes_require_authentication_except_public_test_endpoint(): void
    {
        $this->getJson('/api/ingredients')->assertStatus(401);
        $this->postJson('/api/ingredients', [])->assertStatus(401);
        $this->getJson('/api/ingredients/1')->assertStatus(401);
        $this->putJson('/api/ingredients/1', [])->assertStatus(401);
        $this->deleteJson('/api/ingredients/1')->assertStatus(401);
    }
}
