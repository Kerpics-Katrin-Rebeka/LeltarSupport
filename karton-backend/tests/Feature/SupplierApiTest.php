<?php

namespace Tests\Feature;

use App\Models\PurchaseOrder;

class SupplierApiTest extends ApiTestCase
{
    public function test_authenticated_user_can_list_suppliers(): void
    {
        $this->authenticate();
        $this->createSupplier();
        $this->createSupplier();

        $response = $this->getJson('/api/suppliers');

        $response
            ->assertOk()
            ->assertJsonPath('success', true)
            ->assertJsonCount(2, 'data');
    }

    public function test_authenticated_user_can_create_supplier(): void
    {
        $this->authenticate();

        $response = $this->postJson('/api/suppliers', [
            'name' => 'Fresh Food Ltd',
            'email' => 'fresh@test.com',
            'phone' => '123456789',
        ]);

        $response
            ->assertStatus(201)
            ->assertJsonPath('success', true)
            ->assertJsonPath('data.name', 'Fresh Food Ltd');

        $this->assertDatabaseHas('suppliers', [
            'name' => 'Fresh Food Ltd',
            'email' => 'fresh@test.com',
            'phone' => '123456789',
        ]);
    }

    public function test_create_supplier_validates_required_and_unique_fields(): void
    {
        $this->authenticate();
        $this->createSupplier(['name' => 'Existing Supplier']);

        $response = $this->postJson('/api/suppliers', [
            'name' => 'Existing Supplier',
            'email' => 'not-an-email',
        ]);

        $response
            ->assertStatus(422)
            ->assertJsonValidationErrors(['name', 'email']);
    }

    public function test_authenticated_user_can_show_supplier_with_purchase_orders(): void
    {
        $this->authenticate();
        $supplier = $this->createSupplier();
        PurchaseOrder::create([
            'supplier_id' => $supplier->id,
            'status' => 'pending',
        ]);

        $response = $this->getJson('/api/suppliers/'.$supplier->id);

        $response
            ->assertOk()
            ->assertJsonPath('data.id', $supplier->id)
            ->assertJsonCount(1, 'data.purchase_orders');
    }

    public function test_authenticated_user_can_update_supplier(): void
    {
        $this->authenticate();
        $supplier = $this->createSupplier(['name' => 'Old Supplier']);

        $response = $this->putJson('/api/suppliers/'.$supplier->id, [
            'name' => 'New Supplier',
            'email' => 'new@test.com',
            'phone' => '987654321',
        ]);

        $response
            ->assertOk()
            ->assertJsonPath('success', true)
            ->assertJsonPath('data.name', 'New Supplier');

        $this->assertDatabaseHas('suppliers', [
            'id' => $supplier->id,
            'name' => 'New Supplier',
            'email' => 'new@test.com',
            'phone' => '987654321',
        ]);
    }

    public function test_supplier_cannot_be_deleted_if_it_has_purchase_orders(): void
    {
        $this->authenticate();
        $supplier = $this->createSupplier();
        PurchaseOrder::create([
            'supplier_id' => $supplier->id,
            'status' => 'pending',
        ]);

        $response = $this->deleteJson('/api/suppliers/'.$supplier->id);

        $response
            ->assertStatus(400)
            ->assertJsonPath('success', false)
            ->assertJsonPath('message', 'Supplier has purchase orders and cannot be deleted');
    }

    public function test_authenticated_user_can_delete_supplier_without_purchase_orders(): void
    {
        $this->authenticate();
        $supplier = $this->createSupplier();

        $response = $this->deleteJson('/api/suppliers/'.$supplier->id);

        $response
            ->assertOk()
            ->assertJsonPath('success', true);

        $this->assertDatabaseMissing('suppliers', [
            'id' => $supplier->id,
        ]);
    }

    public function test_supplier_routes_require_authentication(): void
    {
        $this->getJson('/api/suppliers')->assertStatus(401);
        $this->postJson('/api/suppliers', [])->assertStatus(401);
        $this->getJson('/api/suppliers/1')->assertStatus(401);
        $this->putJson('/api/suppliers/1', [])->assertStatus(401);
        $this->deleteJson('/api/suppliers/1')->assertStatus(401);
    }
}
