<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Inventory extends Model
{
    use HasFactory;

    protected $fillable = [
        'ingredient_id',
        'quantity',
        'minimum_level',
    ];

    public $timestamps = false;

    public function ingredient()
    {
        return $this->belongsTo(Ingredient::class);
    }
}
