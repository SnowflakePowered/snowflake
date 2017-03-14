export default (state={}, payload) => {
    switch (payload.type) {
        case 'UPDATE_PLATFORMS':
            return payload.platforms
        default:
            return state;
    }
};