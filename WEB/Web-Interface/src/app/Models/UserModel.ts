export default interface UserModel {
    id:number,
    name:string,
    email:string,
    token:string|undefined,
    pwd:string,
    roles:Role[]
}

export interface response{
  data:{
    user:UserModel,
    token:string
  },
  message:string,
  success:boolean
}

export interface newUser{
  name:string,
  email:string,
  password:string,
  role:string,
}

export interface Role{
  name:string
  id:number
}
