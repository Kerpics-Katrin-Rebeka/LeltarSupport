<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Models\User;
use App\Models\Role;
use Illuminate\Support\Facades\Hash;
use Illuminate\Support\Facades\DB;
use Illuminate\Database\QueryException;

class StaffController extends Controller
{
    /**
     * Display a listing of the resource.
     */
    public function index()
    {
        return User::with('roles')->get();
    }

    /**
     * Store a newly created resource in storage.
     */
    public function store(Request $request)
    {
        $validated = $request->validate([
            'name' => 'required|string|max:255',
            'email' => 'required|string|email|max:255|unique:users',
            'password' => 'required|string|min:8',
            'role' => 'required|string|exists:roles,name',
        ]);

        $role = Role::where('name', $validated['role'])->firstOrFail();

        $newGuy = User::create([
            'name' => $validated['name'],
            'email' => $validated['email'],
            'password_hash' => Hash::make($validated['password']),
        ]);

        $newGuy->roles()->attach($role->id);

        return response()->json([
            'message' => 'Profile for ' . $newGuy->name . ' created successfully',
            'user' => $newGuy,
        ], 201);
    }

    /**
     * Display the specified resource.
     */
    public function show(string $id)
    {
        return User::with('roles')->findOrFail($id);
    }

    /**
     * Update the specified resource in storage.
     */
    public function update(Request $request, string $id)
    {
        $toUpdate = User::findOrFail($id);

        $validated = $request->validate([
            'name' => 'required|string|max:255',
            'email' => 'required|string|email|max:255|unique:users,email,' . $toUpdate->id,
            'pwd' => 'required|nullable|string|min:8',
            'roles' => 'required|array|min:1',
            'roles.*.id' => 'required_with:roles|integer|exists:roles,id',
            'roles.*.name' => 'string|exists:roles,name',
        ]);

        $modData = [];

        if (array_key_exists('name', $validated)) {
            $modData['name'] = $validated['name'];
        }

        if (array_key_exists('email', $validated)) {
            $modData['email'] = $validated['email'];
        }

        $plainPassword = ($validated['pwd'] ?? null);
        if (!empty($plainPassword)) {
            $modData['password_hash'] = Hash::make($plainPassword);
        }

        if (!empty($modData)) {
            $toUpdate->update($modData);
        }

        if (array_key_exists('roles', $validated)) {
            $roleIds = collect($validated['roles'])->pluck('id')->all();
            $toUpdate->roles()->sync($roleIds);
        } elseif (array_key_exists('role', $validated)) {
            $role = Role::where('name', $validated['role'])->firstOrFail();
            $toUpdate->roles()->sync([$role->id]);
        }

        return response()->json($toUpdate->load('roles'));
    }

    /**
     * Remove the specified resource from storage.
     */
    public function destroy(string $id)
    {
        $toDelete = User::with('orders')->findOrFail($id);

        if ($toDelete->orders()->exists()) {
            return response()->json([
                'message' => 'This employee cannot be deleted because they are linked to existing orders.',
            ], 409);
        }

        try {
            DB::transaction(function () use ($toDelete) {
                $toDelete->roles()->detach();
                $toDelete->delete();
            });
        } catch (QueryException $e) {
            return response()->json([
                'message' => 'This employee cannot be deleted because they are linked to other records.',
            ], 409);
        }

        return response()->json(null, 204);
    }

    public function getRoles(){
        return Role::all();
    }
}
