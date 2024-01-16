import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import ValidateForm from './../helper/validateForm';
import { AuthService } from './../../services/auth.service';
import { Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';

@Component({
  selector: 'signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {

  type: string = 'password'
  isText = false;
  eyeIcon: string = "fa-eye-slash"
  SingupForm!: FormGroup;
  public Pass!: string;
  isPassAlpha = false;
  isSpecialChars =false;

  constructor(
       private fb: FormBuilder,
       private auth: AuthService,
       private router: Router,
       private toast: NgToastService) {


  }

  ngOnInit(): void {
    this.SingupForm = this.fb.group({
      firstName:['', Validators.required],
      lastName: ['', Validators.required],
      email:    ['', [Validators.required ,Validators.email]],
      username: ['', Validators.required],
      password: ['', Validators.required],

    })
  }

  checkPassValidate(password: string) {
    const alphanumericRegex = /(?=.*[A-Z])/;
    const specialCharsRegex = /^(?=.*[!"#$%&'()*+,-./:;<=>?@[\]^_\{|}~])[A-Za-z\d!"#$%&'()*+,-./:;<=>?@[\]^_\{|}~]{8,}$/; // At least one special character:
    this.isPassAlpha  = alphanumericRegex.test(password);
    this.isSpecialChars = specialCharsRegex.test(password);

  }

  hideShowPass() {
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = "fa-eye" : this.eyeIcon = "fa-eye-slash";
    this.isText ? this.type = "text" : this.type = "password";

  }




  onSingup() {
    if (this.SingupForm.valid) {
      //send Obj to the Db using Api
      this.auth.signup(this.SingupForm.value)
        .subscribe({
          next: (res => {
           // console.log(this.SingupForm.value);
            this.SingupForm.reset();
            this.toast.success({ detail: "SUCCESS", summary: res.message, duration: 5000});
            this.router.navigate(['/']);

          }),
          error: (err => {
            this.toast.error({detail:"ERROR" , summary: err.error.message, duration: 5000});


          })
        })
    }
    else {
      ValidateForm.validateAllFormFileds(this.SingupForm);
    }

  }





}
