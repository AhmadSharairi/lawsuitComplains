import { animate, state, style, transition, trigger } from "@angular/animations";

export let fade =trigger('slide', [
  transition(':enter', [
    style({ transform: 'translateX(-10px)' }), animate(500),
  ]),
]);


export let fadeInOut =  trigger('fadeInOut', [
  state('void', style({
    opacity: 0
  })),
  transition('void <=> *', animate(1000)),
])
;
