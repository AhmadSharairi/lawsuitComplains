import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ResetPassword } from '../models/reset-password.model';

@Injectable({
  providedIn: 'root'
})
export class ResetPasswordService {
private baseurl: string="https://localhost:7103/api/User/";

constructor( private http:HttpClient) { }

sendResetPasswordLink(email: string)
{
return this.http.post<any>(`${this.baseurl}send-reset-email/${email}`,{})
}

resetPassword(resetPasswordObj: ResetPassword)
{ console.log(resetPasswordObj);


return this.http.post<any>(`${this.baseurl}reset-password`, resetPasswordObj);
}


}
