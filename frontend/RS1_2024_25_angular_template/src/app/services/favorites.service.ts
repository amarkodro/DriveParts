import {MyConfig} from '../my-config';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class FavoritesService {
    private apiUrl = MyConfig.api_address + '/api/Favorites';

    constructor(private http: HttpClient) { }

    toggleFavorite(partId: number): Observable<any> {
        return this.http.post(`${this.apiUrl}/toggle/${partId}`, {});
    }

    getFavorites(): Observable<any[]> {
        return this.http.get<any[]>(`${this.apiUrl}`);
    }

    getFavoriteIds(): Observable<number[]> {
        return this.http.get<number[]>(`${this.apiUrl}/ids`);
    }
}
