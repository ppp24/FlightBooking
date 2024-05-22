var Timer;

Timer = (function () {

    function Timer(opts) {
        this.opts = opts != null ? opts : {};
        _(this).extend(Backbone.Events);
        _.defaults(this.opts, {
            start: 0
        });
        this.tickBank = 0;
        this.cues = [];
    }

    Timer.prototype.start = function () {
        var _this = this;
        Date.now = Date.now || function () { return +new Date; };
        this.tickMark = Date.now();
        this.ticker = setInterval(function () {
            _this.newTickmark = Date.now();
            _this.tickBank += _this.newTickmark - _this.tickMark;
            _this.tickMark = _this.newTickmark;
            _this.trigger('tick', _this.tickBank);
            return _this.checkCues();
        }, 25);
        return this;
    };

    Timer.prototype.stop = function () {
        clearInterval(this.ticker);
        Date.now = Date.now || function () { return +new Date; };
        this.newTickMark = Date.now();
        this.tickBank += this.newTickMark - this.tickMark;
        this.tickMark = this.newTickmark;
        return this;
    };

    Timer.prototype.reset = function () {
        this.tickBank = 0;
        this.clearCues();
        return this;
    };

    Timer.prototype.restart = function () {
        this.reset();
        this.start();
        return this;
    };

    Timer.prototype._normalize = function (val) {
        return Math.floor(val / 100);
    };

    Timer.prototype.checkCues = function () {
        var cue, now, _i, _len, _ref, _results;
        now = this.tickBank;
        _ref = this.cues;
        _results = [];
        for (_i = 0, _len = _ref.length; _i < _len; _i++) {
            cue = _ref[_i];
            if (this._normalize(cue.at) === this._normalize(now)) {
                _results.push(cue.fn());
            } else {
                _results.push(void 0);
            }
        }
        return _results;
    };

    Timer.prototype.seek = function (tickBank) {
        this.tickBank = tickBank;
        return this;
    };

    Timer.prototype.addCue = function (cue) {
        this.cues.push({
            at: cue.at,
            fn: _.debounce(cue.fn, 1000, true)
        });
        return this;
    };

    Timer.prototype.addCues = function (cues) {
        var cue, _i, _len;
        for (_i = 0, _len = cues.length; _i < _len; _i++) {
            cue = cues[_i];
            this.addCue(cue);
        }
        return this;
    };

    Timer.prototype.at = function (tick, fn) {
        this.cues.push({
            at: tick,
            fn: _.debounce(fn, 1000, true)
        });
        return this;
    };

    Timer.prototype.clearCues = function () {
        this.cues = [];
        return this;
    };

    Timer.prototype.msecs = function () {
        return this.tickBank;
    };

    Timer.prototype.secs = function () {
        return Math.floor(this.tickBank / 100) / 10;
    };

    return Timer;

})();