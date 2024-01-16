import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ResetPassword } from './../../models/reset-password.model';
import ValidateForm from './../helper/validateForm';
import { ResetPasswordService } from './../../services/reset-password.service';
import { NgToastService } from 'ng-angular-popup';

@Component({
  selector: 'app-reset',
  templateUrl: './reset.component.html',
  styleUrls: ['./reset.component.css'],
})


export class ResetComponent implements OnInit {
  resetForm!: FormGroup;
  public basePassword!: string;
  public confPassword!: string;
  public isSamePassword!: boolean;
  emailToReset!: string;
  emailToken!: string;
  ResetPasswordObj = new ResetPassword();
  isPassAlpha = false;
  isSpecialChars =false;

  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private resetService: ResetPasswordService,
    private toast: NgToastService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.resetForm = this.fb.group({
      newPassword: [null, Validators.required],
      confirmPassword: [null, Validators.required],
    });

    //*queryParams : Set of key-value pairs that are appended to the end of a URL
    this.activatedRoute.queryParams.subscribe((val) => {
      this.emailToReset = val['email']; //get email value  in the link browser
      let urlToken = val['code'];
      this.emailToken = urlToken.replace(/ /g,'+');
     // console.log(this.emailToReset);
     // console.log(this.emailToken);
    });
  }

  checkIsSamePassword() {
    if (this.basePassword !== this.confPassword)
     this.isSamePassword = false;
    else this.isSamePassword = true;
    return this.isSamePassword;
  }

  checkPassValidate(password: string) {
    const alphanumericRegex = /(?=.*[A-Z])/;
    const specialCharsRegex = /[$&+,:;=?@#|'<>./^*()%!-]/; // At least one special character:
    this.isPassAlpha  = alphanumericRegex.test(password);
    this.isSpecialChars = specialCharsRegex.test(password);
    console.log(this.isPassAlpha);


  }


  Reset() {
    //Send Token to the Database
    if (this.resetForm.valid) {
      this.ResetPasswordObj.email = this.emailToReset;
      this.ResetPasswordObj.newPassword = this.resetForm.value.newPassword;

      this.ResetPasswordObj.emailToken = this.emailToken;
      this.ResetPasswordObj.confirmPassword =this.resetForm.value.confirmPassword;

      this.resetService.resetPassword(this.ResetPasswordObj).
      subscribe({
        next: (res) => {
          this.toast.success({
            summary: 'Password Reset Successfully!',
            duration:3000
           })
           this.router.navigate(['/']);
        },
        error: (err) => {
          console.log('Error:', err);
          let errorMessage = 'Something went wrong';
          if (err.status === 400) {
            errorMessage = 'Invalid request';
          } else if (err.status === 401) {
            errorMessage = 'Unauthorized access';
          } else if (err.status === 404) {
            errorMessage = 'Resource not found';
          }
          this.toast.error({
            detail: err.message || errorMessage,
            summary: 'Error',
            duration: 3000
          });

        }
      });
    } else {
      ValidateForm.validateAllFormFileds(this.resetForm);
    }
  }
}
