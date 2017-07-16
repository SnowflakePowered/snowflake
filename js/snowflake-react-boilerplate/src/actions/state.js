import { STATE_SET_ACTIVE_PLATFORM, createAction } from './'

export const setActivePlatform = platform => createAction(STATE_SET_ACTIVE_PLATFORM)(platform)
