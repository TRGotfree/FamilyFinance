import { Input, Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { MenuItem } from '../menuItem.model';

@Component({
  selector: 'app-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ToolbarComponent implements OnInit {

  constructor() { }

  @Input() menuItems: MenuItem[];

  ngOnInit() {
    if (!this.menuItems || this.menuItems.length === 0) {
      this.menuItems = [
        new MenuItem('/costs', 'Расходы', 'trending_down'),
        new MenuItem('/incomes', 'Доход', 'trending_up'),
        new MenuItem('/reports', 'Отчёты', 'assignment'),
        new MenuItem('/dictionaries', 'Справочники', 'book')
      ];
    }
  }

}
