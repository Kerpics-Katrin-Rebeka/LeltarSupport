<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use App\Models\Inventory;

class AnalyticsController extends Controller
{
    public function reorderSuggestion()
    {
        $lowStock = Inventory::with('ingredient')
            ->whereColumn('quantity','<','minimum_level')
            ->get();

        return response()->json($lowStock);
    }
}
