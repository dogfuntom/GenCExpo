const Victor_favorite = function ($_p) {
    let k = 2;
    let i = 0;

    $_p.setup = function () {
        $_p.createCanvas(400, 400)
    };

    $_p.draw = function () {
        $_p.background(220);
        let k = 2;
        for (step = 0; step <= k; step++) {
            for (let i = 0; i <= k; i++) {
                $_p.rect($_p.width / k * (i % 2), $_p.height / k * i, $_p.width / k, $_p.height / k);
                $_p.fill(250, 180, i);
                $_p.rect($_p.width / k * (i % 2 + 2 * step), $_p.height / k * i, $_p.width / k, $_p.height / k);
                $_p.fill(250, 180, i)
            }
        }
    };
}
