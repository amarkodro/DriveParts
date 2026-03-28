import {MyConfig} from '../my-config';
import { Injectable } from '@angular/core';
import {Observable} from 'rxjs';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private apiUrl = MyConfig.api_address + '/api/Users';

  constructor(private http: HttpClient) { }

  updateUser(id:number, data:any):Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, data);
  }

  editUser(id:number, data:any):Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/update-profile/${id}`, data);
  }
  getUserById(id: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${id}`);
  }

  updateInfo(id: number, data: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/update-info/${id}`, data);
  }

}
