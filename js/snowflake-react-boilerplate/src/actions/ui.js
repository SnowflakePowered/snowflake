import { UI_SET_ACTIVE_PLATFORM, beginCreateAction } from './actions'

export const setActivePlatform = platform => beginCreateAction(UI_SET_ACTIVE_PLATFORM)(platform)
