export const saveToken = (t: string) => localStorage.setItem("token", t);
export const getToken = () => localStorage.getItem("token");
export const authHeader = () => ({ Authorization: `Bearer ${getToken()}` });
export const logout = () => localStorage.removeItem("token");
