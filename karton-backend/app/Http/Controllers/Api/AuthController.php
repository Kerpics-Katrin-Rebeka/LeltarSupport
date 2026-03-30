<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Hash;
use App\Models\User;

class AuthController extends Controller
{
    public function login(Request $request)
    {
        $toreturn = $request->all();

        $request->validate([
            'email'=>'required|email',
            'password'=>'required|string'
        ]);

        $user = User::where('email', $request->email)->first();
        $returnUser = $user;
        if(!$user || !Hash::check($request->password, $user->password_hash)){
            return response()->json(['message'=>$toreturn, 'user'=> $returnUser], 401);
        }
        else{
            $token = $user->createToken('api-token')->plainTextToken;
        }

        return response()->json([
            'user'=>$user,
            'token'=>$token
        ]);
    }

    public function logout(Request $request)
    {
        $request->user()->currentAccessToken()->delete();
        return response()->json(['message'=>'Logged out']);
    }
}
