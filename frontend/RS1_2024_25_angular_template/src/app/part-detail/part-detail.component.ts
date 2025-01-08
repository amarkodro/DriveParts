import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PartService } from '../services/navbar-search.service';

@Component({
  selector: 'app-part-detail',
  templateUrl: './part-detail.component.html',
  styleUrls: ['./part-detail.component.css']
})
export class PartDetailComponent implements OnInit {
  partId: string | null = null;
  part: any = null;

  constructor(private route: ActivatedRoute, private partService: PartService) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((paramMap) => {
      this.partId = paramMap.get('id');
      console.log('Part ID:', this.partId);
      if (this.partId) {
        this.fetchPartDetails(this.partId);
      }
    });
  }

  fetchPartDetails(id: string): void {
    this.partService.getPartById(id).subscribe(
      (data) => {
        console.log('Fetched part details:', data); //
        this.part = data;
      },
      (error) => {
        console.error('Error fetching part details:', error);
      }
    );
  }
}
