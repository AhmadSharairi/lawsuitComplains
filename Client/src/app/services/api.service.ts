import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';



@Injectable({
  providedIn: 'root'
})
export class ApiService {

  private baseurl: string = "http://localhost:5164/api/Admin/AllComplains"; // connect to the Api

  constructor(private http: HttpClient) { }


  getAllComplains() {
        return this.http.get<any>(this.baseurl);
  }



}
