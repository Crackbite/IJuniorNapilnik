# Clean Code & Code Style

> Непонятность функции часто связана со смешением уровней абстракции.
> Если следовать DIP (он преимущественно говорит о том, что не нужно
> лазать вверх или вниз), то многое станет проще

> Деды в своё время показывали друг другу милые мини-программы, которые
> добавляют курсору эффект блесток из мультиков про феечек и перенимали
> из них опыт

> Тело ифов например, зачастую, рекомендуют делать из вызова
> какой-нибудь функции. По своей практике считаю это сомнительным, но
> бывает это хорошо заходит

> Функция должна выполнять только одну операцию. За одну операцию
> считается несколько этапов на одном уровне абстракции. Очень легко это
> воспринимать как паттерн стратегия, когда функция формирует стратегию
> на своём уровне

> Переформулировка кода - это не изменение уровня абстракции

> Функции запутываются, когда в них смешиваются уровни абстракции

> В функции можно использовать функции её уровня и на один ниже

Проведите рефакторинг - [CleanCode_ExampleTask21-27.cs](CleanCode_ExampleTask21-27.cs)