Polymer('materia-gameview-details', {
			created: function() {
				this.games = snowflake.snowflakeApi.games;
				global.debug.gameview = this;
			},
			domReady: function() {
				this.game = snowflake.snowflakeApi.games[0];
			},
			ready: function(){
			},
			gameSelect: function(e, detail) {
				try{
					if (detail.isSelected) {
						this.game = this.games[detail.item.getAttribute('index')];
						  var animation = this.$.slideleft;
						  animation.target = this.$.wrapperGameTitle;
						  animation.play();
					}
				}catch(err){
				}
			}
		});