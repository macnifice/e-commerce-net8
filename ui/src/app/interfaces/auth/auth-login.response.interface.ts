import { User } from "../user/user.interface";

export interface AuthLoginResponse {
  accessToken: string;
  refreshToken: string;
  user: User;
}
