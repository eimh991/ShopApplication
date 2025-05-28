Как загрузить .sql файл в PostgreSQL через DBeaver
1. Открой DBeaver на другом компьютере
Убедись, что PostgreSQL установлен и запущен.

Открой DBeaver.

2. Подключись к PostgreSQL
В меню Database → New Database Connection → выбери PostgreSQL.

Введи:

host: localhost (или IP сервера),

port: 5432,

database: postgres (или нужную тебе),

user: postgres,

password: твоя_пароль.

3. Создай пустую базу данных (если ещё нет)
В панели слева, щёлкни правой кнопкой мыши по подключению → Create → Database.

Назови, например, MySmallShop.

4. Загрузить .sql файл
Вариант 1: через "Execute SQL Script"
Щёлкни правой кнопкой мыши по базе MySmallShop →
Tools → Execute script (Инструменты → Выполнить скрипт).

В появившемся редакторе:

Нажми 📂 "Open File" вверху → выбери файл MySmallShop.sql.

Убедись, что в выпадающем списке вверху выбрана база MySmallShop.

Нажми ▶ (Execute) или Ctrl+Enter.

Вариант 2: через SQL Editor вручную
Щёлкни правой кнопкой по MySmallShop → SQL Editor → New SQL Script.

Открой MySmallShop.sql в блокноте, скопируй всё содержимое.

Вставь в редактор DBeaver.

Нажми ▶ (или Ctrl + Enter).

✅ После выполнения скрипта база будет восстановлена: таблицы, данные и т. д.

Если хочешь — могу помочь проверить структуру или ошибки при импорте.