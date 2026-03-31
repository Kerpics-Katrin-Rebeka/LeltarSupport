export default interface UserModel {
    name:string,
    email:string,
    token:string
}

export interface response{
  user:UserModel,
  token:string
}

export interface newUser{
  name:string,
  email:string,
  password:string,
  role:string
}