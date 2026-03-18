<?php
protected $middlewareGroups = [
    'api' => [
        \App\Http\Middleware\Cors::class,
        'throttle:api',
        \Illuminate\Routing\Middleware\SubstituteBindings::class,
    ],
];