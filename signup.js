
    document.getElementById('loginForm').addEventListener('submit', function (e) {
        e.preventDefault(); // يمنع الإرسال التلقائي

        // الحصول على القيم
        const firstName = document.getElementById('f_name').value.trim();
        const lastName = document.getElementById('l_name').value.trim();
        const email = document.getElementById('email').value.trim();
        const userId = document.getElementById('userId').value.trim();
        const password = document.getElementById('password').value;
        const rePassword = document.getElementById('re_password').value;

        // التحقق من تعبئة الحقول
        if (!firstName || !lastName || !email || !userId || !password || !rePassword) {
            alert("يرجى ملء جميع الحقول.");
            return;
        }

        // التحقق من صحة البريد الإلكتروني
        const emailPattern = /^[^ ]+@[^ ]+\.[a-z]{2,3}$/;
        if (!emailPattern.test(email)) {
            alert("يرجى إدخال بريد إلكتروني صحيح.");
            return;
        }

        // التحقق من تطابق كلمتي المرور
        if (password !== rePassword) {
            alert("كلمتا المرور غير متطابقتين.");
            return;
        }

        // التحقق من قوة كلمة المرور
        const passwordPattern = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$/;
        if (!passwordPattern.test(password)) {
            alert("كلمة المرور يجب أن تحتوي على:\n- حرف كبير\n- حرف صغير\n- رقم\n- رمز خاص (!@#$...)\n- 6 أحرف على الأقل");
            return;
        }

        // نجاح التسجيل
        alert("تم التسجيل بنجاح!");
        document.getElementById('loginForm').reset();
    });
      document.querySelector('.facebook').addEventListener('click', function () {
        alert("جاري التحويل إلى Facebook... (تسجيل دخول اجتماعي غير مفعل حاليًا)");
    });

    document.querySelector('.google').addEventListener('click', function () {
        alert("جاري التحويل إلى Google... (تسجيل دخول اجتماعي غير مفعل حاليًا)");
    });

    document.querySelector('.apple').addEventListener('click', function () {
        alert("جاري التحويل إلى Apple... (تسجيل دخول اجتماعي غير مفعل حاليًا)");
    });

    document.querySelector('.instagram').addEventListener('click', function () {
        alert("جاري التحويل إلى Instagram... (تسجيل دخول اجتماعي غير مفعل حاليًا)");
    });

