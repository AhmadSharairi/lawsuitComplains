import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { SignupComponent } from './components/signup/signup.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { NgToastModule } from 'ng-angular-popup';
import { TokenInterceptor } from './Interceptors/token.interceptor';
import { AuthGuard } from './Guard/auth.guard';
import { ResetComponent } from './components/reset/reset.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ComplaintComponent } from './components/complaint/complaint.component';



const routes: Routes = [
  { path: '', component:  LoginComponent , title:'Login'},
  { path: 'signup', component: SignupComponent, title:'SignUp' },
  { path: 'dashboard', component: DashboardComponent , canActivate:[AuthGuard] , title:'Dashboard'},
  { path: 'reset', component: ResetComponent, title:'Reset Password' },
];

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    SignupComponent,
    DashboardComponent,
    ResetComponent,
    ComplaintComponent ,


]
 ,

  imports: [
    BrowserModule,
    RouterModule.forRoot(routes),
    ReactiveFormsModule,
    FormsModule,
    HttpClientModule,
    NgToastModule,
    BrowserAnimationsModule //Add Animations


  ],

  providers: [
   {
    //config the tokenInterceptor step2
    provide:HTTP_INTERCEPTORS,
    useClass: TokenInterceptor,
    multi:true
  }



  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
