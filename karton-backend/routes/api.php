<?php

use Illuminate\Http\Request;
use Illuminate\Support\Facades\Route;
use App\Http\Controllers\Api\AuthController;
use App\Http\Controllers\Api\ProductController;
use App\Http\Controllers\Api\IngredientController;
use App\Http\Controllers\Api\InventoryController;
use App\Http\Controllers\Api\OrderController;
use App\Http\Controllers\Api\AnalyticsController;
use App\Http\Controllers\Api\PurchaseOrderController;
use App\Http\Controllers\Api\SupplierController;

Route::post('/login', [AuthController::class, 'login']);
Route::post('/register', [AuthController::class, 'register']); 


Route::middleware('auth:sanctum')->group(function () {

    Route::post('/logout', [AuthController::class, 'logout']);
    Route::post('/refresh', [AuthController::class, 'refresh']);

    Route::apiResource('products', ProductController::class);   


    Route::apiResource('ingredients', IngredientController::class);

    Route::get('inventory', [InventoryController::class, 'index']); 
    Route::get('inventory/{ingredient}', [InventoryController::class, 'show']); 
    Route::post('inventory/{ingredient}/adjust', [InventoryController::class, 'update']); 


    Route::apiResource('orders', OrderController::class);

    Route::apiResource('suppliers', SupplierController::class);
    Route::apiResource('purchase-orders', PurchaseOrderController::class);


    Route::get('analytics/reorder', [AnalyticsController::class, 'reorderSuggestion']);
    Route::get('analytics/stock', [AnalyticsController::class, 'stockSummary']); 
});
