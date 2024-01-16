import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { TokenApiModel } from '../models/token-api.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private baseUrl: string = 'http://localhost:5164/api/Auth/'; // connect to the Api

  private userPayload: any;

  constructor(private http: HttpClient, private router: Router) {
    this.userPayload = this.decodeToken();
  }

  // POST send to the backend result Obj and use fun in OnSignUp
  signup(userObj: any) {
    return this.http.post<any>(`${this.baseUrl}register`, userObj); // post to the register Api
  }

  // send to the backend result Obj

  loginPost(loginObj: any) {
    return this.http.post<any>(`${this.baseUrl}login`, loginObj); // post to the  authenticate Api
  }

  // **********************Token functions***********************

  // store Token From BACKEND
  storeToken(tokenValue: string) {
    localStorage.setItem('token', tokenValue);
  }
  //get Token From BACKEND

  getToken() {
    const tokenHere = localStorage.getItem('token');
    return localStorage.getItem('token');
  }

  // store Refresh Token from Backend
  storeRefreshToken(tokenValue: string) {
    localStorage.setItem('refreshToken', tokenValue);
  }

  //Get Refresh Token From Backend
  getRefreshToken() {
    return localStorage.getItem('refreshToken');
  }

  //is Logged In the user or not
  isLoggedIn(): boolean {
    return !!localStorage.getItem('token'); //if user have token that make the user is loggedIn and have token, each user have diffrent token
  }

  // **********************Token functions************************

  SignOut() {
    localStorage.clear();
    this.router.navigate(['/']);
  }

  // Decode Token role and username from before that must be this command -- > npm i @auth0/angular-jwt
  decodeToken() {
    const jwtHelper = new JwtHelperService();
    const token = this.getToken()!;
    console.log(jwtHelper.decodeToken(token));
    return jwtHelper.decodeToken(token); // return payload data as obj from token
  }

  getFullNameFromToken() {
    if (this.userPayload) return this.userPayload.name;
  }

  getRoleFromToken() {
    if (this.userPayload) return this.userPayload.role;
  }

  renewToken(tokenApi: TokenApiModel) {
    return this.http.post<any>(`${this.baseUrl}refresh`, tokenApi);
  }
}
