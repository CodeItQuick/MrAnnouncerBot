﻿class SplatSprites extends Sprites {
  longDrips: Sprites;
  mediumDrips: Sprites;
  shortDrips: Sprites;

  constructor(color: string, expectedFrameCount: number, frameInterval: number, animationStyle: AnimationStyle, padFileIndex: boolean = false, hitFloorFunc?, onLoadedFunc?) {
    super(`Paint/Splats/${color}/Splat`, expectedFrameCount, frameInterval, animationStyle, padFileIndex, hitFloorFunc, onLoadedFunc);
    this.longDrips = new Sprites(`Paint/Drips/Long/${color}/Drip`, 153, 15, AnimationStyle.SequentialStop, true);
    this.mediumDrips = new Sprites(`Paint/Drips/Medium/${color}/Drip`, 138, 15, AnimationStyle.SequentialStop, true);
    this.shortDrips = new Sprites(`Paint/Drips/Short/${color}/Drip`, 119, 15, AnimationStyle.SequentialStop, true);
  }

  draw(context: CanvasRenderingContext2D, now: number): number {
		super.draw(context, now);
		let numSpritesDrawn: number = 0;
    numSpritesDrawn += this.longDrips.draw(context, now);
    numSpritesDrawn += this.mediumDrips.draw(context, now);
		numSpritesDrawn += this.shortDrips.draw(context, now);
		return numSpritesDrawn;
  }
}