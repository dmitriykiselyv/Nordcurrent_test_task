# Nordcurrent test task
Для виконання завдання використовуйте версію
Unity - 2022.3.8f1

Якщо у завданні є деталі, які не вказані - це означає, що вони залишені на
розгляд розробника.

# Завдання:
Створіть гру, в якій гравець керує танком.
Противник - ШІ (декілька інших танків).
Ігрове поле являє собою прямокутник.
Якщо гравець намагається вийти за межі ігрового поля - натикається на
“стіну”.
Головна мета гри - знищення танків ШІ за допомогою пострілів та
запобігання зіткнення з танками ШІ.
Знищений танк має відновитися заново в одному з кутів ігрового поля через
секунду після знищення.
Танки ШІ рухаються довільно на площині під будь-яким кутом, але змінюють
напрямок через певні проміжки часу.
Зверніть увагу, що така зміна напрямку руху під час повороту має відбуватися
плавно з визначеним у параметрах радіусом розвороту.
Під час зіткнення з танком ШІ або зі стіною напрямок танку гравця також
змінюється, розворот танку відбувається на місці.
Можливі інші варіанти реалізації зіткнень.
Снаряди рухаються рівномірно і прямолінійно згідно напрямку, у якому його
вистрілив танк гравця.
У разі влучання снаряду у стіну - снаряд знищується.
У разі влучання снаряду в танк ШІ - знищується і снаряд і танк ШІ.
У разі знищення усіх танків ШІ - вони з’являються наново і випадково
обраних точках вздовж кордонів ігрового поля.
Гравець керує своїм танком використовуючи кнопки ASWD (рух) та клік
(постріл).
При цьому, кнопки А і D відповідають за обертання танка проти/за
годинниковою стрілкою. А кнопки W і S - за рух вперед/назад.
Танк гравця може рухатися тільки тоді, коли гравець нажав щонайменше
одну з кнопок, що відповідають за рух.
Постріл відбувається у тому напрямку, в якому знаходився танк в момент
пострілу.
У випадку зіткнення з танком ШІ, танк гравця знищується та відновлюється
наново через 1 секунду в одному з кутів ігрового поля.

# Деталі:

Під час розробки гри необхідно закласти наступні архітектурні можливості:
- зміна модулів ШІ танків;
- зміна принципу руху і керування (наприклад, роздільне керування
гусеницями, тощо);
- додавання пострілів у танки ШІ.

Плюсом буде реалізація збереження та завантаження поточного стану гри у
форматі .json або .xml. А також вирішення конфліктів появи танків гравця та
ШІ.
Графіка, яку ви хочете використати у завданні - не принципова, якщо це не
порушує чиїсь авторські права. Вона має зайняти мінімальну кількість часу
відведеного вами на це завдання.
