export default interface UserModel {
    name:string,
    role:string,
    email:string,
    pwd:string,
    token:string
}

export interface response{
  user:UserModel,
  token:string
}