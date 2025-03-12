export interface User {
  id: number;
  userName: string;
  email: string;
  role: string;
  refreshToken: string;
}

export interface UserInfo {
  name: string;
  lastName: string;
  email: string;
  role: string;
}
