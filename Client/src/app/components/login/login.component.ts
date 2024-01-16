import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import ValidateForm from './../helper/validateForm';
import { AuthService } from './../../services/auth.service';
import { Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import { UserStoreService } from './../../services/user-store.service';
import { ResetPasswordService } from './../../services/reset-password.service';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  type: string = 'password';
  isText = false;
  eyeIcon: string = 'fa-eye-slash';
  public resetPasswordEmail!: string;
  public isValidEmail!: boolean;

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router,
    private toast: NgToastService,
    private userStore: UserStoreService,
    private resetService: ResetPasswordService
  ) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  hideShowPass() {
    this.isText = !this.isText;
    this.isText ? (this.eyeIcon = 'fa-eye') : (this.eyeIcon = 'fa-eye-slash');
    this.isText ? (this.type = 'text'): (this.type = 'password');
  }

  onLogin() {
    if (this.loginForm.valid) {
      //send Obj Data to the Db using Api
      this.auth.loginPost(this.loginForm.value).subscribe({
        next: (res) => {
          this.loginForm.reset();
          this.auth.storeToken(res.accessToken);
          this.auth.storeRefreshToken(res.refreshToken);

          // To make name in token refresh auto when user login
          const tokenPayLoad = this.auth.decodeToken();
          this.userStore.setFullNameFromStore(tokenPayLoad.name);
          this.userStore.setRoleFromStore(tokenPayLoad.role);
          // use npm i ng-angular-popup --force  Cli_command to use toast inasted of Alert First
          this.toast.success({
            detail: 'SUCCESS',
            summary: res.message,
            duration: 5000,
          });
          this.router.navigate(['/dashboard']);
        },
        error: (err) => {
          this.toast.error({
            detail: 'ERROR',
            summary: 'Username or password not correct!',
            duration: 5000,
          });
          console.log(err);
        },
      });
    } else {
      ValidateForm.validateAllFormFileds(this.loginForm);
    }
  }


  checkValidEmail(event: string) {
    const value = event;
    const pattren = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,3}$/;
    this.isValidEmail = pattren.test(value);

    return this.isValidEmail;
  }



  confirmToSend() {
    if (this.checkValidEmail(this.resetPasswordEmail)) {
      this.resetService.sendResetPasswordLink(this.resetPasswordEmail)
      .subscribe({
        next:(res)=>{
          this.toast.success({
            summary: 'Password reset link has been sent to your email!',
            duration:5500
           })
          this.resetPasswordEmail = "";
          const buttonRef = document.getElementById('closeBtn');
          buttonRef?.click();
        },
        error:(err)=>{
            this.toast.error({
              detail:'Error',
              summary: 'Something Went Wrong',
              duration:3000,

            })
        }

      })











    }
  }
}
