import { Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LocationsModel } from '../models/location';

@Injectable({
  providedIn: 'root'
})
export class Location {
     private apiUrl = 'http://localhost:5135/api/Location';

  constructor(private http: HttpClient) {}

  getLocations(): Observable<LocationsModel[]> {
    return this.http.get<LocationsModel[]>(this.apiUrl);
  }
}
