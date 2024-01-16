import { Component } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { UserStoreService } from 'src/app/services/user-store.service';
import {trigger,state,style,animate,transition,} from '@angular/animations';
import { fade, fadeInOut } from 'src/app/animations';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
  animations: [fade ,fadeInOut],
})

export class DashboardComponent {
  public complains: any = [];
  public fullName: string = '';
  status: boolean = false;
  constructor(
    private auth: AuthService,
    private api: ApiService,
    private userStore: UserStoreService
  ) {}
  ngOnInit() {
    this.api.getAllComplains().subscribe((res) => {
      this.complains = res;
    });

    this.userStore.getFullNameFromStore().subscribe((val) => {
      let fullNameFromToken = this.auth.getFullNameFromToken();
      this.fullName = val || fullNameFromToken;
    });
  }

  Logout() {
    this.auth.SignOut();
  }

  clickEvent() {
    this.status = !this.status;
  }


}
